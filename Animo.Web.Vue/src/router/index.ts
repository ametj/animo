import AuthTokenStore from "@/store/AuthTokenStore";
import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import Home from "@/views/Home.vue";
import EmptyLayout from "@/layouts/EmptyLayout.vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import RouteNames from "./RouteNames";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "index",
    redirect: "/home",
    component: DefaultLayout,
    children: [
      {
        path: "home",
        name: RouteNames.Home,
        component: Home,
      },
      {
        path: "about",
        name: RouteNames.About,
        meta: { requiresAuth: true },
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "about" */ "../views/About.vue"),
      },
    ],
  },
  {
    path: "/account",
    name: "Account",
    component: EmptyLayout,
    children: [
      {
        path: "login",
        name: RouteNames.Login,
        component: () => import("../views/login/Login.vue"),
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to, _) => {
  if (to.matched.some((record) => record.meta.requiresAuth) && !AuthTokenStore.isAuthenticated()) {
    return {
      name: RouteNames.Login,
      query: { redirect: to.fullPath },
    };
  }
});

export default router;
