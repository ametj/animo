export default class AuthTokenStore {
    private static readonly storageKey: string = 'token';

    public static getToken() {
        return localStorage.getItem(AuthTokenStore.storageKey);
    }

    public static setToken(token: string) {
        localStorage.setItem(AuthTokenStore.storageKey, token);
    }

    public static removeToken(): void {
        localStorage.removeItem(AuthTokenStore.storageKey);
    }

    public static isAuthenticated(): boolean {
        return !!AuthTokenStore.getToken();
    }

    public static getTokenData() {
        const token = AuthTokenStore.getToken();
        if (token) {
            return JSON.parse(atob(token.split('.')[1]));
        }

        return {};
    }
}