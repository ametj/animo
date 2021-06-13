interface IResponse<T = any> {
  data: T | undefined;
  hasErrors: boolean;
  errors: object;
}
