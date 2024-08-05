import objectToFormConverter from "../funs/ObjectToFormConverter";
import INotesGetParams from "../interfaces/parametrs/INotesGetParams";
import INoteDto from "../interfaces/dtos/INoteDto";
import INoteChangeDto from "../interfaces/dtos/INoteChangeDto";
import { api } from "..";
import { AxiosResponse } from "axios";
import Note from "../classes/Note";

export async function notesGet(
  params: INotesGetParams
): Promise<AxiosResponse<Note[]>> {
  const path = `/notes?from=${isNaN(params.from ?? 0) ? "" : params.from}&to=${
    isNaN(params.to ?? 0) ? "" : params.to
  }&sortType=${params.sortType}&search=${params.search}`;
  return api.get<Note[]>(path).then();
}

export async function noteGet(id: number): Promise<AxiosResponse<Note>> {
  return api.get<Note>(`/notes/${id}`).then();
}

//Ебн html5 не позволяет отправлять header-ы в форме, пиздец нахуй!!! ПОтому FormData вместо формы.
export async function noteCreate(note: INoteDto): Promise<AxiosResponse<void>> {
  return api.post(`/notes/`, objectToFormConverter(note)).then();
}

export async function noteRemove(id: number): Promise<AxiosResponse<void>> {
  return api.delete(`/notes/${id}`).then();
}

export async function noteUpdate(
  note: INoteChangeDto
): Promise<AxiosResponse<void>> {
  return api.put(`/notes/${note.Id}`, objectToFormConverter(note)).then();
}
