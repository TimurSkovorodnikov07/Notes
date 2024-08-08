import objectToFormConverter from "../funs/ObjectToFormConverter";
import IUserLoginDto from "../interfaces/dtos/IUserLoginDto";
import IUserRegistrationDto from "../interfaces/dtos/IUserRegistrationDto";
import IAccountCreateResponse from "../interfaces/responses/IAccountCreateResponse";
import { api } from "..";
import { AxiosResponse } from "axios";
import ILoginResponse from "../interfaces/responses/ILoginResponse";
import ICodeResendResponse from "../interfaces/responses/ICodeResendResponse";
import { User } from "../classes/User";
import ITokens from "../interfaces/ITokens";

export async function accountCreate(
  dto: IUserRegistrationDto
): Promise<AxiosResponse<IAccountCreateResponse>> {
  //КОроче, форму чтобы отправить нужно ее в качетсве 2 парр а не писать в {data}
  return api
    .post<IAccountCreateResponse>("/accountcreate", objectToFormConverter(dto))
    .then();
}
export async function login(
  dto: IUserLoginDto
): Promise<AxiosResponse<ILoginResponse>> {
  return api.post<ILoginResponse>("/login", objectToFormConverter(dto)).then();
}

export async function codeResend(
  userId: string
): Promise<AxiosResponse<ICodeResendResponse>> {
  return api.put<ICodeResendResponse>(`/coderesend/${userId}`).then();
}

export async function userinfo(): Promise<AxiosResponse<User>> {
  return api.get("/userinfo").then();
}

export async function emailVerify(
  userId: string,
  code: string
): Promise<AxiosResponse<ITokens>> {
  return api
    .post<ITokens>(`/emailverify`, {
      UserId: userId,
      Code: code,
    })
    .then();
}
