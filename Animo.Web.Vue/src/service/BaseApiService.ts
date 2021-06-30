import AuthTokenStore from "@/store/AuthTokenStore";
import axios, { AxiosError, AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";

export default class BaseApiService {
  protected readonly api: AxiosInstance;

  constructor() {
    this.api = axios.create({ baseURL: this.apiUrl });
    this.api.interceptors.request.use((request) => this.onRequest(request));
    this.api.interceptors.response.use(
      (response) => this.toOkResponse(response),
      (error) => this.toErrorResponse(error)
    );
  }

  public get apiUrl(): string {
    var apiUrl = process.env.VUE_APP_APIURL;
    return apiUrl?.endsWith("/") ? apiUrl : `${apiUrl}/`;
  }

  public async get<T>(url: string, data: any): Promise<IResponse<T>> {
    return await this.api.get<T>(url, data);
  }

  public async post<T>(url: string, data: any): Promise<IResponse<T>> {
    return await this.api.post<T>(url, data);
  }

  public async put<T>(url: string, data: any): Promise<IResponse<T>> {
    return await this.api.put<T>(url, data);
  }

  protected toOkResponse<T>(response: AxiosResponse<T>): any {
    return { data: response.data, hasErrors: false, errors: [] } as IResponse<T>;
  }

  protected toErrorResponse(error: AxiosError): any {
    if (!error.response) return;

    const status = error.response?.status;
    if (status === 400) {
      return { hasErrors: true, errors: error.response.data.errors } as IResponse;
    } else if (status === 401) {
      AuthTokenStore.removeToken();
    } else if (status === 404) {
      return { hasErrors: true } as IResponse;
    }
  }

  private onRequest = (request: AxiosRequestConfig) => {
    request.headers.Authorization = `Bearer ${AuthTokenStore.getToken()}`;

    return request;
  };
}
