import { Component, OnInit } from "@angular/core";
import { QuanLyDieu } from "src/app/corelogic/interface/admin/quanlydieu.model";
import { QuanlydieuService } from "src/app/corelogic/model/common/admin/quanlydieu.service";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { ToastrService } from "ngx-toastr";
import { initFlowbite } from "flowbite";
import * as FileSaver from "file-saver";
import * as XLSX from "xlsx";

@Component({
  selector: "app-quanlydieu",
  templateUrl: "./quanlydieu.component.html",
  styleUrls: ["./quanlydieu.component.css"],
})
export class QuanlydieuComponent {
  title = "Quản Lý Điều";
  quanLyKhoaData: QuanLyDieu[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenKhoa: boolean = true;
  isMakhoa: boolean = true;
  tenkhoa: string = "";
  addKhoa = new QuanLyDieu();
  editingkhoa: QuanLyDieu = new QuanLyDieu();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  isIcon = true;
  isIcons = true;
  successMessage: string = "";
  isPopupVisible = true;
  showModal: boolean = false;
  showModalAdd: boolean = false;
  namek = "Tên Điều";
  showpassword: boolean = false;
  showExcel: boolean = false;
  checkDowload = false;
  checkimport = false;
  selectedFileName: string = "";
  ondelete: boolean = false;
  selectedLop: any;
  loading = false;
  showDeleteAllButton: boolean = false;
  selectedRowCount: number = 0;
  onalldelete = false;
  checkExport = false;
  activeButton: string | null = null;
  filter1 = false;
  showdetail = false;
  showNH = false;
  slectedNH = "";
  dropNH: any;
  showNHE = false;
  slectedNHE = "";

  constructor(
    private dieuService: QuanlydieuService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.dieuService.getDieuData().subscribe((data) => {
      this.quanLyKhoaData = data.data;
      this.selectedItems = new Array(data.length).fill(false);
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });
    this.loadData();
  }
  resetSelection() {
    this.selectedItems = new Array(this.quanLyKhoaData.length).fill(false);
    this.selectedRowCount = 0;
    this.checkAll = false;
    this.showDeleteAllButton = false;
  }

  selectOne(event: any, i: number): void {
    this.selectedItems[i] = event.target.checked;
    this.selectedRowCount = this.selectedItems.filter((item) => item).length;
    this.checkAll = this.selectedRowCount === this.quanLyKhoaData.length;
    this.showDeleteAllButton = this.selectedRowCount >= 2;
  }

  checkIfAnyRowSelected(): boolean {
    const selectedCount = this.selectedItems.filter((item) => item).length;
    return selectedCount >= 2;
  }

  checkAllRows(): void {
    this.checkAll = !this.checkAll;
    const selectedCount = this.quanLyKhoaData.length;
    if (this.checkAll) {
      this.selectedItems = new Array(selectedCount).fill(true);
      this.selectedRowCount = selectedCount;
    } else {
      this.selectedItems = new Array(selectedCount).fill(false);
      this.selectedRowCount = 0;
    }
    this.showDeleteAllButton = this.checkIfAnyRowSelected();
  }
  deleteSelectedRows(): void {
    const selectedRows = this.selectedItems
      .map((selected, index) => (selected ? index : -1))
      .filter((index) => index !== -1);
    if (selectedRows.length > 0) {
      selectedRows.forEach((index) => {
        const selectedLop = this.quanLyKhoaData[index];
        if (selectedLop) {
          this.dieuService.deleteDieu(selectedLop.iddieu).subscribe(() => {
            this.quanLyKhoaData = this.quanLyKhoaData.filter(
              (lop) => lop.iddieu !== selectedLop.iddieu
            );
            this.selectedItems[index] = false;
            this.selectedRowCount--;
            this.showDeleteAllButton = this.selectedRowCount > 0;
            this.checkAll = false;
            this.loadData();
          });
        }
      });
    }
  }

  onDowload() {
    this.checkDowload = true;
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    this.selectedFileName = file ? file.name : "";
    this.checkimport = !!this.selectedFileName;
  }

  onContinue() {
    if (this.checkimport) {
      this.importExcel();
      this.closeExcels();
    }
  }

  showExcels() {
    this.showExcel = true;
  }

  closeExcels() {
    this.showExcel = false;
  }

  cautrucExcel() {
    const element = document.getElementById("excel-table");
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    XLSX.writeFile(wb, "File_QuanLyKhoa.xlsx");
  }

  importExcel() {
    const fileInput = document.getElementById(
      "dropzone-file"
    ) as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      const file = fileInput.files[0];
      const fileReader = new FileReader();

      fileReader.onload = (e: any) => {
        const binaryString = fileReader.result;
        const workbook = XLSX.read(binaryString, { type: "binary" });
        const sheetNameList = workbook.SheetNames;
        const sheetName = sheetNameList[0];
        const excelData = XLSX.utils.sheet_to_json(workbook.Sheets[sheetName], {
          raw: false,
        }) as QuanLyDieu[];
        for (const item of excelData) {
          this.dieuService
            .postDataToDatabase(item)
            .pipe(
              catchError((error) => {
                if (error.status === 400) {
                  this.toast.error(
                    "Dữ liệu không hợp lệ. Vui lòng kiểm tra và thử lại."
                  );
                  return error;
                } else {
                  this.toast.error(
                    "Có lỗi khi thêm dữ liệu. Vui lòng thử lại."
                  );
                  return error;
                }
              })
            )
            .subscribe(() => {
              this.loadData();
            });
        }
        this.toast.success("Thêm dữ liệu thành công");
      };

      fileReader.readAsBinaryString(file);
    }
  }

  exportToExcel() {
    // Lấy dữ liệu từ danh sách
    const dataWithoutId = this.quanLyKhoaData.map((item) => {
      const { iddieu, ...rest } = item;
      return rest;
    });

    // Tạo một worksheet từ dữ liệu
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);

    // Thêm viền cho từng ô trong worksheet
    const range = XLSX.utils.decode_range(worksheet["!ref"] || "");
    for (let R = range.s.r; R <= range.e.r; ++R) {
      for (let C = range.s.c; C <= range.e.c; ++C) {
        const cell_address = { c: C, r: R };
        const cell_ref = XLSX.utils.encode_cell(cell_address);

        // Thêm viền cho mỗi ô
        worksheet[cell_ref].s = {
          border: {
            top: { style: "thin" },
            right: { style: "thin" },
            bottom: { style: "thin" },
            left: { style: "thin" },
          },
        };
      }
    }

    // Tạo một workbook từ worksheet
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet['A1'].v = 'Tên văn bản';
    worksheet['B1'].v = 'Mã điều';
    worksheet['C1'].v = 'Nội dung';
    worksheet['D1'].v = 'Điểm cơ bản';
    worksheet['E1'].v = 'Điểm tối đa';

    // Ghi dữ liệu vào buffer
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });

    // Lưu file excel
    this.saveAsExcelFile(excelBuffer, "quanlydieu.xlsx");
  }

  // saveAsExcelFile(buffer: any, fileName: string) {
  //   const data: Blob = new Blob([buffer], { type: "application/octet-stream" });
  //   const url: string = window.URL.createObjectURL(data);
  //   const a: HTMLAnchorElement = document.createElement("a");
  //   a.href = url;
  //   a.download = fileName;
  //   document.body.appendChild(a);
  //   a.click();
  //   document.body.removeChild(a);
  //   window.URL.revokeObjectURL(url);
  // }
  saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], { type: "application/octet-stream" });
    FileSaver.saveAs(data, fileName);
  }

  sortDataByTenkhoa(order: "asc" | "desc" = "asc") {
    this.quanLyKhoaData.sort((a: any, b: any) => {
      const comparison = a.tenKhoa.localeCompare(b.tenKhoa);
      return order === "asc" ? comparison : -comparison;
    });
  }
  sortDataByMakhoa() {
    this.isMakhoa = !this.isMakhoa;
    this.isIcon = !this.isIcon;
    this.quanLyKhoaData.sort((a: any, b: any) => {
      if (this.isMakhoa) {
        return a.mKkhoa.localeCompare(b.maKhoa);
      } else {
        return b.maKhoa.localeCompare(a.maKhoa);
      }
    });
  }

  Sort() {
    this.isTenKhoa = !this.isTenKhoa;
    this.isIcons = !this.isIcons;
    this.sortDataByTenkhoa(this.isTenKhoa ? "asc" : "desc");
  }
  loadData() {
    this.loading = true;
    this.dieuService.getDieuData().subscribe((data) => {
      this.quanLyKhoaData = data.data;
      this.checkExport = this.quanLyKhoaData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }

  detail(lop: QuanLyDieu) {
    this.editingkhoa = { ...lop };
    this.showdetail = true;
  }
  closeModalD() {
    this.showdetail = false;
  }
  filterKhoa(slectedKhoa: any) {
    this.tenkhoa = slectedKhoa;
    this.namek = "Điều:" + this.tenkhoa;
  }
  resetFilter() {
    this.tenkhoa = "";
    this.namek = "Tất Cả Điều";
  }
  resetModal() {
    this.addKhoa = new QuanLyDieu();
    this.showModal = false;
  }

  addDieuMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()+{}\[\]:;<>,?~\\/]/);

    if (
      !this.addKhoa.maDieu ||
      !this.addKhoa.tenVanBan ||
      !this.addKhoa.noiDung ||
      !this.addKhoa.diemCoBan ||
      !this.addKhoa.diemToiDa
    ) {
      this.toast.warning("Vui lòng điền đầy đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.addKhoa.maDieu)) {
      this.toast.warning("Mã điều không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addKhoa.tenVanBan)) {
      this.toast.warning("Tên điều không được chứa ký tự đặc biệt!");
      return;
    }
    if (isNaN(Number(this.addKhoa.diemToiDa))) {
      this.toast.warning("Điểm tối đa phải là một số!");
      return;
    }
    if (isNaN(Number(this.addKhoa.diemCoBan))) {
      this.toast.warning("Điểm cơ bản phải là một số!");
      return;
    }
    this.dieuService.postThemDieu(this.addKhoa).subscribe((data) => {
      this.addKhoa = new QuanLyDieu();
      this.toast.success("Thêm thành công");
      this.loadData();
      this.resetModal();
      this.closeModalAdd();
    });
  }

  editDieu(khoa: QuanLyDieu) {
    this.editingkhoa = { ...khoa };
    this.showModal = true;
  }
  closeModal() {
    this.showModal = false;
  }
  showAdd() {
    this.showModalAdd = true;
  }
  closeModalAdd() {
    this.showModalAdd = false;
  }

  toogleClick() {
    this.showpassword = !this.showpassword;
  }

  updateDieu() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()+{}\[\]:;<>,?~\\/]/);
    if (
      !this.editingkhoa.maDieu ||
      !this.editingkhoa.tenVanBan ||
      !this.editingkhoa.noiDung ||
      !this.editingkhoa.diemCoBan ||
      !this.editingkhoa.diemToiDa
    ) {
      this.toast.warning("Vui lòng điền đầy đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.editingkhoa.maDieu)) {
      this.toast.warning("Mã khoa không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingkhoa.tenVanBan)) {
      this.toast.warning("Tên khoa không được chứa ký tự đặc biệt!");
      return;
    }
    if (isNaN(Number(this.editingkhoa.diemToiDa))) {
      this.toast.warning("Điểm tối đa phải là một số!");
      return;
    }
    if (isNaN(Number(this.editingkhoa.diemCoBan))) {
      this.toast.warning("Điểm cơ bản phải là một số!");
      return;
    }
    if (this.editingkhoa) {
      this.dieuService.putEditDieu(this.editingkhoa).subscribe((data) => {
        const index = this.quanLyKhoaData.findIndex(
          (k) => k.iddieu === this.editingkhoa.iddieu
        );
        if (index !== -1) {
          this.quanLyKhoaData[index] = { ...this.editingkhoa };
        }
        this.toast.success("Cập nhật thành công");
        this.loadData();
        this.closeModal();
      });
    }
  }

  onDelete(idkhoa: any) {
    this.selectedLop = idkhoa;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteDieu() {
    if (this.selectedLop) {
      this.dieuService.deleteDieu(this.selectedLop).subscribe(() => {
        this.quanLyKhoaData = this.quanLyKhoaData.filter(
          (lop) => lop.iddieu !== this.selectedLop
        );
        this.selectedLop = null;
        this.ondelete = false;
        this.toast.success("Xoá thành công");
        this.loadData();
      });
    }
  }

  onDeleteAll() {
    this.onalldelete = true;
  }
  closeDeleteAll() {
    this.onalldelete = false;
  }
  deleteLopAll() {
    this.deleteSelectedRows();
    this.onalldelete = false;
  }
  toggleDropDown() {
    this.filter1 = !this.filter1;
    this.activeButton = "FT Tuần";
    if (this.filter1 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
  clickshowNH() {
    this.showNH = !this.showNH;
  }
  slecteddropNH(option: string) {
    this.slectedNH = option;
    this.addKhoa.tenVanBan = option;
    this.showNH = false;
  }
  clickshowNHE() {
    this.showNHE = !this.showNHE;
  }
  slecteddropNHE(option: string) {
    this.slectedNHE = option;
    this.editingkhoa.tenVanBan = option;
    this.showNHE = false;
  }
}
