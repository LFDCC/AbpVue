<template>
  <el-form
    ref="aForm"
    :model="userInfo"
    :rules="aRules"
  >
    <el-form-item
      :label="$t('AbpAccount[\'DisplayName:UserName\']')"
      prop="userName"
    >
      <el-input v-model.trim="userInfo.userName" />
    </el-form-item>
    <el-form-item :label="$t('AbpAccount[\'DisplayName:Name\']')" prop="name">
      <el-input v-model.trim="userInfo.name" />
    </el-form-item>
    <el-form-item :label="$t('AbpAccount[\'DisplayName:Email\']')" prop="email">
      <el-input v-model.trim="userInfo.email" />
    </el-form-item>
    <el-form-item
      :label="$t('AbpAccount[\'DisplayName:PhoneNumber\']')"
      prop="phoneNumber"
    >
      <el-input v-model.trim="userInfo.phoneNumber" />
    </el-form-item>
    <el-form-item :label="$t('AbpVue.PersonIntroduction')">
      <el-input
        v-model.trim="userInfo.extraProperties.Introduction"
        type="textarea"
        :rows="2"
        :placeholder="$t('AbpVue.Input', [$t('AbpVue.PersonIntroduction')])"
      />
    </el-form-item>
    <el-form-item>
      <el-button :loading="loading" type="primary" @click="submit">{{
        $t("AbpAccount['Submit']")
      }}</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import { mapGetters } from "vuex";
export default {
  name: "Profile",
  data() {
    return {
      aRules: {
        name: [
          {
            required: true,
            message: this.$i18n.t("AbpAccount['ThisFieldIsRequired.']"),
            trigger: ["blur", "change"],
          },
        ],
        email: [
          {
            required: true,
            message: this.$i18n.t("AbpAccount['ThisFieldIsRequired.']"),
            trigger: ["blur", "change"],
          },
        ],
        userName: [
          {
            required: true,
            message: this.$i18n.t("AbpAccount['ThisFieldIsRequired.']"),
            trigger: ["blur", "change"],
          },
        ],
        phoneNumber: [
          {
            required: true,
            message: this.$i18n.t("AbpAccount['ThisFieldIsRequired.']"),
            trigger: ["blur", "change"],
          },
        ],
      },
      loading: false,
      userInfo: {
        userName:  this.$store.getters.username,
        email: this.$store.getters.email,
        name: this.$store.getters.name,
        phoneNumber: this.$store.getters.phoneNumber,
        extraProperties: {
          Avatar: this.$store.getters.avatar,
          Introduction: this.$store.getters.introduction,
        },
      },
    };
  },
  computed: {
    ...mapGetters([
      "avatar",
    ]),
  },
  watch: {
    avatar(val) {
      this.userInfo.extraProperties.Avatar = val;
    },
  },
  methods: {
    submit() {
      this.$refs.aForm.validate((valid) => {
        if (valid) {
          this.loading = true;
          this.$store
            .dispatch("user/setUserInfo", this.userInfo)
            .then((res) => {
              this.loading = false;
              this.$notify({
                title: this.$i18n.t("AbpVue['Success']"),
                message: this.$i18n.t("AbpVue['SuccessMessage']"),
                type: "success",
                duration: 2000,
              });
              this.loading = false;
            })
            .catch(() => {
              this.loading = false;
            });
        } else {
          return false;
        }
      });
    },
  },
};
</script>
