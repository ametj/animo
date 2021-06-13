import BaseApiService from "./BaseApiService";
import AuthTokenStore from "@/store/AuthTokenStore";
import { ref, Ref } from "vue";

class AuthService extends BaseApiService {
  private readonly _userName: Ref<string>;

  constructor() {
    super();
    this._userName = ref(AuthTokenStore.getTokenData()?.sub);
  }

  async login(loginForm: ILogin): Promise<IResponse<ILoginToken>> {
    const response = await this.post<ILoginToken>("account/login", loginForm);
    if (response.data?.token) {
      AuthTokenStore.setToken(response.data.token);
      this.setUserName();
    }
    return response;
  }

  logout() {
    AuthTokenStore.removeToken();
    this.setUserName();
  }

  public get userName(): Ref<string> {
    return this._userName;
  }

  private setUserName() {
    this._userName.value = AuthTokenStore.getTokenData()?.sub;
  }
}

export default new AuthService();
