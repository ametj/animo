import { defineComponent } from "vue";
import { useRoute, useRouter } from "vue-router";
import AuthService from "@/service/AuthService";
import RouteNames from "@/router/RouteNames";
import { useI18n } from "vue-i18n";
import useForm from "@/composables/form";

export default defineComponent({
  setup() {
    const router = useRouter();
    const route = useRoute();
    const { t } = useI18n();

    const { form, formData, errors, submitAction, submit } = useForm<ILogin>();

    const rules = {
      userNameOrEmail: [{ required: true, message: t("validation.required", [t("account.userNameOrEmail")]) }],
      password: [{ required: true, message: t("validation.required", [t("account.password")]) }],
    };

    submitAction.action = () => {
      AuthService.login(formData).then((response) => {
        if (!response.hasErrors) {
          const redirect = route.query.redirect ? (route.query.redirect as string) : { name: RouteNames.Home };
          router.push(redirect);
        } else {
          errors.value = response.errors;
        }
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
