export default interface IEmailVerifyParams {
  userId: string;
  codeDiedAfterSeconds: number;
  codeLength: number;
}
