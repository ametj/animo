<template>
  <el-row type="flex" justify="center" align="middle" style="height: 100%">
    <el-col :xs="22" :sm="12" :lg="8" :xl="6">
      <el-form :model="loginForm" @keyup.enter="login">
        <el-form-item
          prop="userNameOrEmail"
          :rules="[{ required: true, message: $t('requiredField', [$t('account.userNameOrEmail')]) }]"
        >
          <el-input v-model="loginForm.userNameOrEmail" :placeholder="$t('account.userNameOrEmail')"></el-input>
        </el-form-item>
        <el-form-item
          prop="password"
          :rules="[{ required: true, message: $t('requiredField', [$t('account.password')]) }]"
        >
          <el-input v-model="loginForm.password" :placeholder="$t('account.password')" show-password></el-input>
        </el-form-item>
        <div v-for="(value, name) in errors" :key="name" style="margin-bottom: 22px">
          <el-alert :title="value[0]" type="error" show-icon></el-alert
          ><!-- {{ $t(name) }} -->
        </div>
        <el-form-item>
          <el-button round type="primary" @click="login">Login</el-button>
        </el-form-item>
      </el-form>
    </el-col>
  </el-row>
</template>

<script src="./Login.ts" lang="ts" />
