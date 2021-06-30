<template>
  <el-form :model="formData" :rules="rules" ref="form" @keyup.enter="submit">
    <el-form-item prop="userNameOrEmail">
      <el-input
        v-model="formData.userNameOrEmail"
        maxlength="30"
        :placeholder="$t('account.userNameOrEmail')"
      ></el-input>
    </el-form-item>
    <el-form-item prop="password">
      <el-input
        v-model="formData.password"
        maxlength="30"
        :placeholder="$t('account.password')"
        show-password
      ></el-input>
    </el-form-item>
    <div v-for="(value, name) in errors" :key="name" style="margin-bottom: 22px">
      <el-alert :title="value[0]" type="error" show-icon></el-alert>
    </div>
    <el-form-item>
      <el-button round type="primary" @click="submit">{{ $t("account.login") }}</el-button>
    </el-form-item>

    <el-form-item>
      <router-link :to="{ name: RouteNames.Register }" style="margin-right: 20px">
        <el-button type="text">{{ $t("account.register") }}</el-button>
      </router-link>
      <router-link :to="{ name: RouteNames.ForgotPassword }">
        <el-button type="text">{{ $t("account.forgotPassword") }}</el-button>
      </router-link>
    </el-form-item>
  </el-form>
</template>

<script src="./Login.ts" lang="ts" />
