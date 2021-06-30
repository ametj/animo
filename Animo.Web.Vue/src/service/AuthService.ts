import BaseApiService from "./BaseApiService";
import AuthTokenStore from "@/store/AuthTokenStore";
import { ref, Ref } from "vue";

class AuthService extends BaseApiService {
  private readonly _userName: Ref<string>;

  constructor() {
    super();
    this._userName = ref(AuthTokenStore.getTokenData()?.sub);
  }

  public async login(form: ILogin): Promise<IResponse<ILoginToken>> {
    const response = await this.post<ILoginToken>("account/login", form);
    if (response.data?.token) {
      AuthTokenStore.setToken(response.data.token);
      this.setUserName();
    }
    return response;
  }

  public logout(): void {
    AuthTokenStore.removeToken();
    this.setUserName();
  }

  public async register(form: IRegister): Promise<IResponse> {
    return await this.post("account/register", form);
  }

  public async forgotPassword(form: IForgotPassword): Promise<IResponse> {
    return await this.post("account/password/reset", form);
  }

  public get userName(): Ref<string> {
    return this._userName;
  }

  private setUserName(): void {
    this._userName.value = AuthTokenStore.getTokenData()?.sub;
  }
}

export default new AuthService();
