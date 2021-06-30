<template>
  <el-form :model="formData" :rules="rules" ref="form" @submit.prevent="submit">
    <el-form-item prop="password">
      <el-input v-model="formData.password" maxlength="32" :placeholder="$t('account.passwordNew')" show-password></el-input>
    </el-form-item>
    <div v-for="(value, name) in errors" :key="name" style="margin-bottom: 22px">
      <el-alert :title="value[0]" type="error" show-icon></el-alert>
    </div>
    <el-form-item>
      <el-button round type="primary" @click="submit">{{ $t("account.changePassword") }}</el-button>
    </el-form-item>
  </el-form>
</template>

<script src="./ResetPassword.ts" lang="ts" />