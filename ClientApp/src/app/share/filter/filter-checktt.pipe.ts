// status-check.pipe.ts
import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "statusCheck",
})
export class StatusCheckPipe implements PipeTransform {
  transform(data: any[]): boolean {
    if (!data || data.length === 0) {
      return true;
    }

    return data.every((item) => !item.trangthai);
  }
}
