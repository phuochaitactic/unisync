import { Pipe, PipeTransform } from "@angular/core";
import { DatePipe } from "@angular/common";

@Pipe({
  name: "dateformat",
})
export class DateFormatPipe implements PipeTransform {
  transform(value: string): string {
    return "*".repeat(value.length);
  }
}
