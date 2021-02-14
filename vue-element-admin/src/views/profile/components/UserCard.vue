<template>
  <el-card style="margin-bottom: 20px;">
    <div slot="header" class="clearfix">
      <span>{{ $t("AbpVue.AboutMe") }}</span>
    </div>

    <div class="user-profile">
      <div class="box-center">
        <el-image
          class="avatar"
          :src="this.avatarFullPath"
          :preview-src-list="[this.avatarFullPath]"
        >
        </el-image>
      </div>
      <div />
      <div class="box-center">
        <div class="user-name text-center">{{ this.username }}</div>
        <div class="user-role text-center text-muted">
          {{ this.roles.join(" | ") }}
        </div>
      </div>
      <div class="box-center">
        <el-upload
          action
          name="file"
          :before-upload="beforeUpload"
          :http-request="uploadAvatar"
          :show-file-list="false"
          :disabled="loading"
        >
          <el-button :loading="loading" type="primary" icon="el-icon-upload">{{
            $t("AbpVue.ChangeAvatar")
          }}</el-button>
        </el-upload>
      </div>
    </div>
    <div class="user-bio">
      <div class="user-education user-bio-section">
        <div class="user-bio-section-header">
          <svg-icon icon-class="education" />
          <span>{{ $t("AbpVue.PersonIntroduction") }}</span>
        </div>
        <div class="user-bio-section-body">
          <div class="text-muted">
            {{
              this.introduction
                ? this.introduction
                : $t("AbpVue.PersonIntroduction")
            }}
          </div>
        </div>
      </div>
    </div>
  </el-card>
</template>

<script>
import { mapGetters } from "vuex";
import { saveAvatar } from "@/api/identity/profile";
export default {
  name: "Profile",
  data() {
    return {
      loading: false,
    };
  },
  computed: {
    ...mapGetters(["username", "roles", "avatarFullPath", "introduction"]),
  },
  methods: {
    beforeUpload(file) {
      const avatarForamt = this.$store.getters.abpConfig.setting.values[
        "AbpVue.AllowedUserAvatar.Formats"
      ];
      const avatarSize = this.$store.getters.abpConfig.setting.values[
        "AbpVue.AllowedUserAvatar.Size"
      ];
      const name = file.name;
      const type = file.type;
      const size = file.size;
      const ext = name.substr(name.lastIndexOf("."));

      if (avatarForamt.indexOf(ext) < 0) {
        this.$message.error(this.$i18n.t("File['File.ErrorFormat']"));
        return false;
      }
      if (size > avatarSize * 1024) {
        this.$message.error(
          this.$i18n.t("File['File.ErrorSize']", [avatarSize])
        );
        return false;
      }

      return true;
    },
    uploadAvatar(data) {
      this.loading = true;
      const fd = new FormData();
      fd.append("file", data.file);
      saveAvatar(fd)
        .then((resData) => {
          this.$store.commit("user/SET_AVATAR", resData.bolbName);
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
    },
  },
};
</script>

<style lang="scss" scoped>
.box-center {
  margin: 0 auto;
  display: table;
}

.text-muted {
  color: #777;
}

.user-profile {
  .avatar {
    width: 150px;
    height: 150px;
    border-radius: 50%;
    display: inline-block;
    position: relative;
    cursor: default;
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
  }
  .user-name {
    font-weight: bold;
  }

  .box-center {
    padding-top: 10px;
  }

  .user-role {
    padding-top: 10px;
    font-weight: 400;
    font-size: 14px;
  }

  .box-social {
    padding-top: 30px;

    .el-table {
      border-top: 1px solid #dfe6ec;
    }
  }

  .user-follow {
    padding-top: 20px;
  }
}

.user-bio {
  margin-top: 20px;
  color: #606266;

  span {
    padding-left: 4px;
  }

  .user-bio-section {
    font-size: 14px;
    padding: 15px 0;

    .user-bio-section-header {
      border-bottom: 1px solid #dfe6ec;
      padding-bottom: 10px;
      margin-bottom: 10px;
      font-weight: bold;
    }
  }
}
</style>
