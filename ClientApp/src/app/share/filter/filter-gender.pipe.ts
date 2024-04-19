import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "gender",
})
export class FilterGenderPipe implements PipeTransform {
  transform(value: boolean): string {
    return value ? "Nam" : "Nữ";
  }
}
