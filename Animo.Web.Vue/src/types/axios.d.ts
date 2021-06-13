import axios from 'axios'

declare module 'axios' {
  export interface AxiosInstance {
    request<T = any> (config: AxiosRequestConfig): Promise<IResponse<T>>;
    get<T = any>(url: string, config?: AxiosRequestConfig): Promise<IResponse<T>>;
    delete<T = any>(url: string, config?: AxiosRequestConfig): Promise<IResponse<T>>;
    head<T = any>(url: string, config?: AxiosRequestConfig): Promise<IResponse<T>>;
    post<T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<IResponse<T>>;
    put<T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<IResponse<T>>;
    patch<T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<IResponse<T>>;
  }
}