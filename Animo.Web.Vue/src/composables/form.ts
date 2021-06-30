import { reactive, ref } from "vue";

interface FormSubmitAction {
  action: () => void;
}

export default function useForm<T extends object>() {
  const form = ref(null);
  const formData = reactive({} as T);
  const errors = ref({});
  const submitAction = {} as FormSubmitAction;

  const submit =  () => {
    errors.value = {};
    (form.value as any).validate((isValid: boolean) => {
      if (isValid) submitAction?.action();
    });
  };

  return { form, formData, errors, submitAction, submit };
}
