import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "uniqueFilter",
})
export class UniqueFilterPipe implements PipeTransform {
  transform(
    data: any[],
    propertyName: string,
    isBoolean: boolean = false
  ): any[] {
    const uniqueValues: any[] = [];
    const seenValues: { [key: string]: boolean } = {};

    for (const item of data) {
      const propertyValue = isBoolean
        ? item[propertyName]
        : item[propertyName].toString();

      if (!seenValues[propertyValue]) {
        seenValues[propertyValue] = true;
        uniqueValues.push(item);
      }
    }

    return uniqueValues;
  }
}
