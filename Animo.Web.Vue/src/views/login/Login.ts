import { defineComponent, reactive, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import AuthService from "@/service/AuthService";

export default defineComponent({
  setup() {
    const router = useRouter();
    const route = useRoute();

    const loginForm = reactive({} as ILogin);
    const errors = ref({});

    const login = () => {
      AuthService.login(loginForm).then((response) => {
        if (!response.hasErrors) {
          const redirect = route.query.redirect ? (route.query.redirect as string) : { name: "home" };
          router.push(redirect);
        } else {
          errors.value = response.errors;
        }
      });
    };

    return {
      loginForm,
      errors,
      login,
    };
  },
});
