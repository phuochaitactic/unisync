import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "filter",
})
export class FilterPipe implements PipeTransform {
  transform(list: any[], property: string, value: string) {
    return value ? list.filter((item) => item[property] === value) : list;
  }
}
