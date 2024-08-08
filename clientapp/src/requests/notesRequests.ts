import objectToFormConverter from "../funs/ObjectToFormConverter";
import INotesGetParams from "../interfaces/parametrs/INotesGetParams";
import INoteDto from "../interfaces/dtos/INoteDto";
import INoteChangeDto from "../interfaces/dtos/INoteChangeDto";
import { api } from "..";
import { AxiosResponse } from "axios";
import Note from "../classes/Note";
import INoteGetResponse from "../interfaces/responses/INotesGetResponse";

export async function notesGet(
  params: INotesGetParams
): Promise<AxiosResponse<INoteGetResponse>> {
  let path = `/notes?from=${params.from}&to=${params.to}&sortType=${params.sortType}`;
  path +=
    params.search != undefined || params.search != ""
      ? `&search=${params.search}`
      : "";
  return api.get<INoteGetResponse>(path).then();
}

export async function noteGet(id: string): Promise<AxiosResponse<Note>> {
  return api.get<Note>(`/notes/${id}`).then();
}

//Ебн html5 не позволяет отправлять header-ы в форме, пиздец нахуй!!! ПОтому FormData вместо формы.
export async function noteCreate(note: INoteDto): Promise<AxiosResponse<void>> {
  return api.post(`/notes/`, objectToFormConverter(note)).then();
}

export async function noteRemove(id: string): Promise<AxiosResponse<void>> {
  return api.delete(`/notes/${id}`).then();
}

export async function noteUpdate(
  note: INoteChangeDto
): Promise<AxiosResponse<void>> {
  return api.put(`/notes`, objectToFormConverter(note)).then();
}
