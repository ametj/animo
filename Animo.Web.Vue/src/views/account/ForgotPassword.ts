import { defineComponent, ref } from "vue";
import AuthService from "@/service/AuthService";
import RouteNames from "@/router/RouteNames";
import { useI18n } from "vue-i18n";
import useForm from "@/composables/form";

export default defineComponent({
  setup() {
    const { t } = useI18n();

    const emailSent = ref(false);

    const { form, formData, errors, submitAction, submit } = useForm<IForgotPassword>();

    const rules = {
      userNameOrEmail: [{ required: true, message: t("validation.required", [t("account.userNameOrEmail")]) }],
    };

    submitAction.action = () => {
      AuthService.forgotPassword(formData).then((response) => {
        if (!response.hasErrors) {
          emailSent.value = true;
        } else {
          errors.value = { userNameOrEmail: [t("account.userNotRegistered")] };
        }
      });
    };

    return {
      RouteNames,
      form,
      formData,
      emailSent,
      rules,
      errors,
      submit,
    };
  },
});
