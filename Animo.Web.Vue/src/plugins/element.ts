import {
  ElButton,
  ElContainer,
  ElHeader,
  ElMain,
  ElFooter,
  ElMenu,
  ElMenuItem,
  ElForm,
  ElFormItem,
  ElInput,
  ElCol,
  ElRow,
  ElLink,
  ElSpace,
} from "element-plus";
import { App } from "vue";

export default (app: App<Element>) => {
  app
    .use(ElButton)
    .use(ElContainer)
    .use(ElHeader)
    .use(ElMain)
    .use(ElFooter)
    .use(ElMenu)
    .use(ElMenuItem)
    .use(ElForm)
    .use(ElFormItem)
    .use(ElInput)
    .use(ElCol)
    .use(ElRow)
    .use(ElLink)
    .use(ElSpace);
};
