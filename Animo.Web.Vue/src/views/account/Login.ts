import { defineComponent, reactive, Ref, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import AuthService from "@/service/AuthService";
import RouteNames from "@/router/RouteNames";
import { useI18n } from "vue-i18n";

export default defineComponent({
  setup() {
    const router = useRouter();
    const route = useRoute();
    const { t } = useI18n();

    const form = ref(null);
    const formData = reactive({} as ILogin);
    const errors = ref({});

    const rules = {
      userNameOrEmail: [{ required: true, message: t("validation.required", [t("account.userNameOrEmail")]) }],
      password: [{ required: true, message: t("validation.required", [t("account.password")]) }],
    };

    const login = () => {
      AuthService.login(formData).then((response) => {
        if (!response.hasErrors) {
          const redirect = route.query.redirect ? (route.query.redirect as string) : { name: RouteNames.Home };
          router.push(redirect);
        } else {
          errors.value = response.errors;
        }
      });
    };

    const submit = () => {
      (form.value as any).validate((isValid: boolean) => {
        if (isValid) login();
      });
    };

    return {
      RouteNames,
      form,
      formData,
      rules,
      errors,
      submit,
    };
  },
});
