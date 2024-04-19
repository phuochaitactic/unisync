import { Component } from "@angular/core";
import { QuanLyVanBan } from "src/app/corelogic/interface/admin/quanlyvanban.model";
import { QuanlyvanbanService } from "src/app/corelogic/model/common/admin/quanlyvanban.service";
import { initFlowbite } from "flowbite";
import { PdfBreakpoints } from "ngx-extended-pdf-viewer";
import { ToastrService } from "ngx-toastr";
@Component({
  selector: "app-danhsachvb-qd",
  templateUrl: "./danhsachvb-qd.component.html",
  styleUrls: ["./danhsachvb-qd.component.css"],
})
export class DanhsachvbQdComponent {
  title = "Quản Lý Văn Bản";
  quanLyCosoData: QuanLyVanBan[] = [];
  selectedFile: File | null = null;
  documents: { name: string }[] = [];
  pdfContent: any;
  addLop = new QuanLyVanBan();
  editingLop: QuanLyVanBan = new QuanLyVanBan();
  showModal: boolean = false;
  showModalAdd: boolean = false;
  showPdf = false;
  showEdit = false;
  selectedVanBan: any;
  loading = false;
  selectedLop: any;
  ondelete: boolean = false;
  searchText = "";
  tenkhoa: string = "";
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  selectedRowCount: number = 0;
  showDeleteAllButton: boolean = false;
  p: number = 1;
  stt: number = 1;
  isIcon = true;
  isIcons = true;
  isTenKhoa: boolean = true;
  isMakhoa: boolean = true;
  activeButton: string | null = null;
  onalldelete = false;
  namek = "Tên Văn Bản";
  filter1 = false;

  constructor(
    private vanbanService: QuanlyvanbanService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    PdfBreakpoints.md = 610;
    this.loading = true;
    initFlowbite();
    this.vanbanService.getVanBanData().subscribe((data: any) => {
      this.quanLyCosoData = data.data;
      this.selectedItems = new Array(data.length).fill(false);
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });
  }
  resetSelection() {
    this.selectedItems = new Array(this.quanLyCosoData.length).fill(false);
    this.selectedRowCount = 0;
    this.checkAll = false;
    this.showDeleteAllButton = false;
  }

  selectOne(event: any, i: number): void {
    this.selectedItems[i] = event.target.checked;
    this.selectedRowCount = this.selectedItems.filter((item) => item).length;
    this.checkAll = this.selectedRowCount === this.quanLyCosoData.length;
    this.showDeleteAllButton = this.selectedRowCount >= 2;
  }

  checkIfAnyRowSelected(): boolean {
    const selectedCount = this.selectedItems.filter((item) => item).length;
    return selectedCount >= 2;
  }

  checkAllRows(): void {
    this.checkAll = !this.checkAll;
    const selectedCount = this.quanLyCosoData.length;
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
        const selectedLop = this.quanLyCosoData[index];
        if (selectedLop) {
          this.vanbanService.deleteVB(selectedLop.id).subscribe(() => {
            this.quanLyCosoData = this.quanLyCosoData.filter(
              (lop) => lop.id !== selectedLop.id
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

  loadData() {
    this.loading = true;
    this.vanbanService.getVanBanData().subscribe((data) => {
      this.quanLyCosoData = data.data;
      this.resetSelection();
      this.loading = false;
    });
  }

  filterKhoa(slectedKhoa: any) {
    this.tenkhoa = slectedKhoa;
    this.namek = "Khoa:" + this.tenkhoa;
  }
  resetFilter() {
    this.tenkhoa = "";
    this.namek = "Tất Cả Khoa";
  }
  resetModal() {
    this.addLop = new QuanLyVanBan();
    this.showModal = false;
  }

  handleFileSelect(event: any): void {
    const fileInput = event.target;
    const files: FileList | null = fileInput.files;

    if (files && files.length > 0) {
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        this.documents.push({ name: file.name });
      }
      fileInput.value = "";
    }
  }

  onFileSelected(event: any) {
    console.log("File selected event:", event);
    const input = document.getElementById(
      "dropzone-filepdf"
    ) as HTMLInputElement;
    if (input) {
      const file: File | null = input.files?.length ? input.files[0] : null;

      if (file) {
        this.readFileAsBase64(file);
      }
    }
  }
  readFileAsBase64(file: File): void {
    const reader = new FileReader();
    reader.onloadend = () => {
      const base64Content = reader.result as string;
      const actualBase64 = base64Content.split(",")[1];

      if (this.editingLop) {
        this.editingLop.link = actualBase64;
      }
      if (this.addLop) {
        this.addLop.link = actualBase64;
      }
    };

    reader.readAsDataURL(file);
  }

  showPDF(url: string) {
    this.selectedVanBan = {
      link: url,
    };
    this.showPdf = true;
  }
  closePDF(): void {
    this.showPdf = false;
    this.selectedVanBan = null;
  }
  addVbtlMoi() {
    console.log(this.addLop);
    if (!this.addLop.maVanBan || !this.addLop.tenVanBan) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }

    this.vanbanService.postThemVB(this.addLop).subscribe((data) => {
      this.addLop = new QuanLyVanBan();
      this.toast.success("Thêm thành công");
      this.resetModal();
      this.loadData();
      this.closeModalAdd();
    });
  }
  editVbtl(vb: QuanLyVanBan) {
    this.editingLop = { ...vb };
    this.showModal = true;
  }
  updateVbtl() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/);

    if (
      !this.editingLop.maVanBan ||
      !this.editingLop.tenVanBan ||
      !this.editingLop.link
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (this.editingLop) {
      this.vanbanService.putEditVB(this.editingLop).subscribe((data) => {
        const index = this.quanLyCosoData.findIndex(
          (k) => k.id === this.editingLop.id
        );
        if (index !== -1) {
          this.quanLyCosoData[index] = { ...this.editingLop };
        }
        this.toast.success("Sửa thành công");
        this.loadData();
        this.closeModal();
      });
    }
  }
  onDelete(id: any) {
    this.selectedLop = id;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteVbtl() {
    if (this.selectedLop) {
      this.vanbanService.deleteVB(this.selectedLop).subscribe(() => {
        this.quanLyCosoData = this.quanLyCosoData.filter(
          (lop) => lop.id !== this.selectedLop
        );
        this.selectedLop = null;
        this.loadData();
        this.ondelete = false;
        this.toast.success("Xoá thành công");
        this.loadData();
      });
    }
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
}
