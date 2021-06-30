import { defineComponent, reactive, ref } from "vue";
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

    const { form, formData, errors, submitAction, submit } = useForm<IRegister>();


    const rules = {
      userName: [
        { required: true, message: t("validation.required", [t("account.userName")]), trigger: "blur" },
        { min: 5, max: 20, message: t("validation.length", [t("account.userName"), 5, 20]), trigger: "blur" },
      ],
      email: [
        { required: true, type: "email", message: t("validation.required", [t("account.email")]), trigger: "blur" },
      ],
      password: [
        { required: true, message: t("validation.required", [t("account.password")]), trigger: "blur" },
        { min: 6, max: 32, message: t("validation.length", [t("account.password"), 6, 32]), trigger: "blur" },
        {
          pattern: /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?([^\w\s]|[_])).{6,32}$/,
          message: t("validation.password"),
          trigger: "blur"
        },
      ],
    };

    submitAction.action = () => {
      AuthService.register(formData).then((response) => {
        if (!response.hasErrors) {
          router.push({ name: RouteNames.Login, query: route.query });
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
