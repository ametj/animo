import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import i18n from "./i18n";
import installElementPlus from "./plugins/element";
import store from "./store";
import '@/assets/styles/element-theme.scss'

import Default from "./layouts/Default.vue";
import Empty from "./layouts/Empty.vue";

const app = createApp(App)
  .use(store)
  .use(i18n)
  .use(router);
installElementPlus(app);

app.component("DefaultLayout", Default);
app.component("EmptyLayout", Empty);
app.mount("#app");
