import IAddress from '../interfaces/IAddress';

export default class Address implements IAddress {
    private _path;
    private element;

    public getPath(): string {
        return this._path;
    }
    public getElement() {
        return this.element;
    }
    constructor(path: string, element: any) {
        this._path = path;
        this.element = element;
    }
}