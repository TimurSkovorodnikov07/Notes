export default class ValueAndType {
  constructor(val: any, typ: string) {
    this.value = val;
    this.type = typ;
  }
  value: any;
  type: string;
}
