import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "Bcansu",
})
export class FilterBcansuPipe implements PipeTransform {
  transform(value: boolean): string {
    return value ? "Lớp Trưởng" : "Không";
  }
}
