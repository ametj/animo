<template>
  <el-form :model="formData" :rules="rules" ref="form" @keyup.enter="submit">
    <el-form-item prop="userName">
      <el-input v-model="formData.userName" maxlength="20" :placeholder="$t('account.userName')"></el-input>
    </el-form-item>
    <el-form-item prop="email">
      <el-input v-model="formData.email" maxlength="255" :placeholder="$t('account.email')"></el-input>
    </el-form-item>
    <el-form-item prop="password">
      <el-input v-model="formData.password" maxlength="32" :placeholder="$t('account.password')" show-password></el-input>
    </el-form-item>
    <div v-for="(value, name) in errors" :key="name" style="margin-bottom: 22px">
      <el-alert :title="value[0]" type="error" show-icon></el-alert>
    </div>
    <el-form-item>
      <el-button round type="primary" @click="submit">{{ $t("account.register") }}</el-button>
    </el-form-item>
  </el-form>
</template>

<script src="./Register.ts" lang="ts" />