<template>
  <el-menu mode="horizontal">
    <el-row type="flex">
      <el-col :span="12">
        <el-row type="flex" align="middle" justify="start">
          <router-link to="/">Animo</router-link>
          <router-link to="/About">
            <el-menu-item index="1">Browse</el-menu-item>
          </router-link>
        </el-row>
      </el-col>
      <el-col :span="12">
        <el-row type="flex" align="middle" justify="end">
          <el-space :size="24">
            <router-link to="/">
              <el-menu-item index="2">Watchlist </el-menu-item>
            </router-link>
            <router-link to="/About">
              <el-menu-item index="3">Calendar</el-menu-item>
            </router-link>
            <router-link to="/">
              <el-menu-item index="4">Ratings</el-menu-item>
            </router-link>
            <el-button v-if="userName" @click="logout" type="text">{{ userName }}</el-button>
            <router-link v-else :to="{ name: RouteNames.Login }"
              ><el-button type="text">{{ $t("account.login") }}</el-button>
            </router-link>
          </el-space>
        </el-row>
      </el-col>
    </el-row>
  </el-menu>
</template>

<script>
import AuthService from "@/service/AuthService";
import RouteNames from "@/router/RouteNames";
import { useRouter } from "vue-router";

export default {
  setup() {
    const router = useRouter();

    const logout = () => {
      AuthService.logout();
      router.push({ name: RouteNames.Home });
    };

    return {
      RouteNames,
      userName: AuthService.userName,
      logout,
    };
  },
};
</script>
