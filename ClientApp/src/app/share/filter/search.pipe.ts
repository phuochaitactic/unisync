import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "search",
})
export class SearchPipe implements PipeTransform {
  dataNotFound: boolean = false;

  transform(value: any, args?: any): any {
    if (!value) return null;
    if (!args) return value;

    args = args.toLowerCase();

    const filteredData = value.filter((item: any) => {
      return JSON.stringify(item).toLowerCase().includes(args);
    });

    this.dataNotFound = filteredData.length === 0;

    return filteredData;
  }
}
