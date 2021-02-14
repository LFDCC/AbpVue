<template>
  <el-dialog
    v-el-drag-dialog
    :visible="showDialog"
    :title="$t('AbpPermissionManagement.Permissions') + '-' + entityDisplayName"
    custom-class="modal-form"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed"
  >
    <el-form
      v-loading="loading"
      :style="{ height: loading ? '500px' : 'auto' }"
    >
      <el-checkbox
        :value="grantAllCheckBoxCheckAll"
        :indeterminate="grantAllCheckBoxForward"
        @change="onGrantAllClicked"
      >
        {{ $t("AbpPermissionManagement.SelectAllInAllTabs") }}
      </el-checkbox>
      <el-divider />
      <el-tabs v-model="activeTabPane" tab-position="left">
        <el-tab-pane
          v-for="group in permissionGroups"
          :key="group.name"
          :label="group.displayName + ' (' + grantedCount(group) + ')'"
          :name="group.name"
        >
          <el-card shadow="never">
            <div slot="header" class="clearfix">
              <h3>{{ group.displayName }}</h3>
            </div>
            <el-checkbox
              :value="scopeCheckBoxCheckAll(group)"
              :indeterminate="scopeCheckBoxForward(group)"
              @change="
                (checked) =>
                  onCheckScopeAllClicked(
                    checked,
                    group,
                    'permissionTree-' + group.name
                  )
              "
            >
              {{ $t("AbpPermissionManagement.SelectAllInThisTab") }}
            </el-checkbox>
            <el-divider />
            <el-tree
              :ref="'permissionTree-' + group.name"
              show-checkbox
              :check-strictly="true"
              node-key="id"
              :data="group.permissions"
              :default-checked-keys="grantedPermissionKeys(group)"
              default-expand-all
              :expand-on-click-node="false"
              @node-click="handleNodeClick"
              @check-change="
                (permission, checked) =>
                  onPermissionTreeNodeCheckChanged(
                    permission,
                    checked,
                    group,
                    'permissionTree-' + group.name
                  )
              "
            />
          </el-card>
        </el-tab-pane>
      </el-tabs>
      <el-divider />
    </el-form>
    <div slot="footer" class="dialog-footer" v-if="!loading">
      <el-button @click="onFormClosed">
        {{ $t("AbpPermissionManagement['Cancel']") }}
      </el-button>
      <el-button type="primary" @click="onSave" :loading="confirmButtonBusy">
        {{ $t("AbpPermissionManagement['Save']") }}
      </el-button>
    </div>
  </el-dialog>
</template>

<script>
import {
  getPermissions,
  updatePermissions,
} from "@/api/permission/permissions";
import { fetchAppConfig } from "@/utils/abp";

export default {
  name: "PermissionForm",
  props: {
    providerName: {
      type: String,
      required: true,
    },
    providerKey: {
      type: String,
      required: true,
    },
    showDialog: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      loading: false,
      activeTabPane: "",
      confirmButtonBusy: false,
      entityDisplayName: "",
      permissionGroups: new Array(),
    };
  },
  methods: {
    /**
     * 获取权限集合
     */
    handleGetPermissions() {
      this.loading = true;
      this.activeTabPane = "";
      this.permissionGroups.length = 0;
      getPermissions({
        providerName: this.providerName,
        providerKey: this.providerKey,
      })
        .then((res) => {
          this.entityDisplayName = res.entityDisplayName;
          res.groups.map((g) => {
            const group = new PermissionGroup(g.name, g.displayName);
            const parents = g.permissions.filter((p) => p.parentName === null);
            parents.forEach((parent) => {
              const permission = new PermissionItem(
                parent.name,
                parent.displayName,
                parent.isGranted
              );
              permission.isGranted = parent.isGranted;
              permission.disabled = this.isGrantedByOtherProviderName(
                parent.grantedProviders
              );
              permission.grantedProviders = parent.grantedProviders;
              const subPermissions = g.permissions.filter((p) =>
                p.parentName?.startsWith(parent.name)
              );
              this.generatePermission(permission, subPermissions);
              group.addPermission(permission);
            });
            this.permissionGroups.push(group);
          });
          if (this.permissionGroups.length > 0) {
            this.activeTabPane = this.permissionGroups[0].name;
          }
        })
        .finally(() => {
          this.loading = false;
        });
    },

    /** 递归生成子节点
     * @param permission 二级权限树
     * @param permissions 权限列表
     */
    generatePermission(permission, permissions) {
      const subPermissions = permissions.filter(
        (p) => p.parentName !== permission.id
      );
      permissions = permissions.filter((p) => p.parentName === permission.id);
      permissions.forEach((p) => {
        const children = new PermissionItem(p.name, p.displayName, p.isGranted);
        children.isGranted = p.isGranted;
        children.disabled = this.isGrantedByOtherProviderName(
          p.grantedProviders
        );
        const itemSubPermissions = subPermissions.filter(
          (sp) => sp.parentName === p.name
        );
        if (itemSubPermissions.length > 0) {
          this.generatePermission(children, itemSubPermissions);
        }
        permission.createChildren(children);
      });
    },
    /**
     * 校验是否授予过了其他提供者的的权限
     */
    isGrantedByOtherProviderName(grantedProviders) {
      if (grantedProviders.length) {
        return grantedProviders.some(
          (p) => p.providerName !== this.providerName
        );
      }
      return false;
    },
    /**
     * 保存权限
     */
    onSave() {
      const permissionData = new PermissionData();
      this.permissionGroups.forEach((group) => {
        this.updatePermissionByInput(permissionData, group.permissions);
      });
      this.confirmButtonBusy = true;
      let permissionsQuery = {
        providerName: this.providerName,
        providerKey: this.providerKey,
      };
      updatePermissions(permissionsQuery, permissionData)
        .then(() => {
          this.$notify({
            title: this.$i18n.t("AbpVue['Success']"),
            message: this.$i18n.t("AbpVue['SuccessMessage']"),
            type: "success",
            duration: 2000,
          });
          fetchAppConfig(
            permissionsQuery.providerKey,
            permissionsQuery.providerName
          );
          this.onFormClosed();
        })
        .finally(() => {
          this.confirmButtonBusy = false;
        });
    },
    updatePermissionByInput(permissions, items) {
      items.forEach((p) => {
        if (p.isGranted !== p.isGrant) {
          permissions.addPermission(p.id, p.isGrant);
        }
        this.updatePermissionByInput(permissions, p.children);
      });
    },

    /**
     * 窗口关闭事件
     */
    onFormClosed() {
      this.$emit("closed");
    },

    /**
     * 授予所有权限 按钮事件
     */
    onGrantAllClicked(checked) {
      this.permissionGroups.forEach((group) => {
        group.setAllGrant(checked);
        const trees = this.$refs["permissionTree-" + group.name];
        const keys = this.grantedPermissionKeys(group);
        trees[0].setCheckedKeys(keys);
      });
    },

    /**
     * Permission Tree 全选按钮事件
     */
    onCheckScopeAllClicked(checked, group, treeRef) {
      group.setAllGrant(checked);
      const trees = this.$refs[treeRef];
      const keys = this.grantedPermissionKeys(group);
      trees[0].setCheckedKeys(keys);
    },

    /**
     * Permission TreeNode 变更事件
     */
    onPermissionTreeNodeCheckChanged(permission, checked, group, treeRef) {
      PermissionItem.setPermissionGrant(checked, permission);
      if (permission.children.length > 0) {
        const trees = this.$refs[treeRef];
        const keys = this.grantedPermissionKeys(group);
        trees[0].setCheckedKeys(keys);
      }
    },
    handleNodeClick(data, node) {
      !node.disabled && (node.checked = !node.checked);
    },
  },
  watch: {
    showDialog: {
      handler(val) {
        val && this.handleGetPermissions();
      },
    },
  },
  computed: {
    /**
     * 所有已授权数量
     */
    grantAllCount() {
      let count = 0;
      this.permissionGroups.forEach((group) => {
        count += group.grantedCount();
      });
      return count;
    },
    /** 某个权限组已授权节点
     * 用于勾选TreeNode
     */
    grantedPermissionKeys() {
      return (group) => {
        return group.grantedPermissionKeys();
      };
    },
    /**
     * 某个权限组权限数量
     * 用于设定单个Tree的全选CheckBox状态
     */
    permissionCount() {
      return (group) => {
        return group.permissionCount();
      };
    },
    /**
     * 所有权限数量
     */
    permissionAllCount() {
      let count = 0;
      this.permissionGroups.forEach((group) => {
        count += group.permissionCount();
      });
      return count;
    },
    /**
     * 单个Tree的全选CheckBox是否为选中状态
     */
    scopeCheckBoxCheckAll() {
      return (group) => {
        const grantCount = group.grantedCount();
        return grantCount === group.permissionCount();
      };
    },
    /**
     * 单个Tree的全选CheckBox状态是否为预选状态
     */
    scopeCheckBoxForward() {
      return (group) => {
        const grantCount = group.grantedCount();
        return grantCount > 0 && grantCount < group.permissionCount();
      };
    },
    /** 某个权限组已授权数量
     * 用于显示已授权数量
     */
    grantedCount() {
      return (group) => {
        return group.grantedCount();
      };
    },
    /**
     * 授权所有CheckBox是否为选中状态
     */
    grantAllCheckBoxCheckAll() {
      return this.grantAllCount === this.permissionAllCount;
    },
    /**
     * 授权所有CheckBox状态是否为预选状态
     */
    grantAllCheckBoxForward() {
      const grantCount = this.grantAllCount;
      return grantCount > 0 && grantCount < this.permissionAllCount;
    },
  },
};
export class PermissionData {
  constructor(name, isGranted) {
    this.name = name;
    this.isGranted = isGranted;
    this.permissions = new Array();
  }
  addPermission(name, isGranted) {
    this.permissions.push(new PermissionData(name, isGranted));
  }
}
/** element权限树 */
export class PermissionItem {
  constructor(id, label, isGrant) {
    /**权限ID */
    this.id = id;
    /**权限名称 */
    this.label = label;
    /**当前操作的授予权限状态 */
    this.isGrant = isGrant;
    /**已经授予的权限状态 */
    this.isGranted = false;
    /** 是否禁用 */
    this.disabled = false;
    /** 子节点 */
    this.children = new Array();
    /**权限提供者 */
    this.grantedProviders = new Array();
  }
  createChildren(permission) {
    permission.parent = this;
    this.children.push(permission);
  }
  setGrant(grant) {
    this.isGrant = grant;
    // fix bug: 会无限的追踪到跟节点,来进行全部取消授权
    if (this.parent && !this.parent.isGrant) {
      this.parent.setGrant(grant);
    }
  }
  static setPermissionGrant(grant, permission) {
    permission.setGrant(grant);
    if (!grant) {
      permission.children.map((p) => {
        PermissionItem.setPermissionGrant(false, p);
      });
    }
  }
  static setAllPermissionGrant(grant, permission) {
    if (!permission.disabled) {
      permission.setGrant(grant);
    }
    permission.children.map((p) => {
      if (!p.disabled) {
        PermissionItem.setAllPermissionGrant(grant, p);
      }
    });
  }
}
export class PermissionGroup {
  constructor(name, displayName) {
    this.name = "";
    this.displayName = "";
    this.permissions = new Array();
    this.name = name;
    this.displayName = displayName;
  }
  permissionCount() {
    let count = 0;
    count += this.deepPermissionCount(this.permissions);
    return count;
  }
  addPermission(permission) {
    this.permissions.push(permission);
  }
  setAllGrant(grant) {
    this.permissions.map((p) => {
      PermissionItem.setAllPermissionGrant(grant, p);
    });
  }
  grantedPermissionKeys() {
    const keys = new Array();
    this.deepGrantedPermissionKeys(keys, this.permissions);
    return keys;
  }
  grantedCount() {
    let count = 0;
    count += this.deepGrantedCount(this.permissions);
    return count;
  }
  deepGrantedCount(permissions) {
    let count = 0;
    count += permissions.filter((p) => p.isGrant).length;
    permissions.forEach((p) => {
      count += this.deepGrantedCount(p.children);
    });
    return count;
  }
  deepGrantedPermissionKeys(keys, permissions) {
    permissions.forEach((p) => {
      if (p.isGrant) {
        keys.push(p.id);
      }
      this.deepGrantedPermissionKeys(keys, p.children);
    });
  }
  deepPermissionCount(permissions) {
    let count = 0;
    count += permissions.length;
    permissions.forEach((p) => {
      count += this.deepPermissionCount(p.children);
    });
    return count;
  }
}
</script>

<style lang="scss" scoped>
::v-deep .el-dialog__body {
  padding-top: 20px;
  padding-bottom: 5px;
}
</style>
