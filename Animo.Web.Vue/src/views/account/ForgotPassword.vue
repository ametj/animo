<template>
  <el-form v-if="!emailSent" :model="formData" :rules="rules" ref="form" @submit.prevent="submit">
    <el-form-item prop="userNameOrEmail">
      <el-input
        v-model="formData.userNameOrEmail"
        maxlength="30"
        :placeholder="$t('account.userNameOrEmail')"
      ></el-input>
    </el-form-item>
    <div v-for="(value, name) in errors" :key="name" style="margin-bottom: 22px">
      <el-alert :title="value[0]" type="error" show-icon></el-alert>
    </div>
    <el-form-item>
      <el-button round type="primary" @click="submit">{{ $t("account.sendPasswordResetEmail") }}</el-button>
    </el-form-item>
  </el-form>
  <el-result
    v-else
    icon="success"
    :title="$t('account.passwordResetEmailSentTitle')"
    :subTitle="$t('account.passwordResetEmailSentSubTitle')"
  ></el-result>
</template>

<script src="./ForgotPassword.ts" lang="ts" />
