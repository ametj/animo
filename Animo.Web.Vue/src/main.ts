import { createApp } from "vue";
import App from "@/App.vue";
import router from "@/router";
import i18n from "@/i18n";
import installElementPlus from "@/plugins/element";
import "@/assets/styles/element-theme.scss";

const app = createApp(App)
  .use(i18n)
  .use(router);
installElementPlus(app);

app.mount("#app");
