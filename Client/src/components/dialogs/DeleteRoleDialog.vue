<template>
  <v-dialog
    v-model="dialog"
    max-width="400px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        color="error"
        v-t="'core.delete'"
        :loading="loading"
      />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span
            class="headline"
            v-t="'project.role.delete'"
          />
        </v-card-title>
        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'project.role.deleteDescription'"
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            v-t="'core.cancel'"
            @click.stop="dialog = false"
          />
          <v-btn
            text
            type="submit"
            color="error"
            v-t="'core.delete'"
            :loading="loading"
            :disabled="!valid"
            @click.stop="deleteRole"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import { Project } from '../../models'
import RoleModule from '../../store/roles'

@Component
export default class DeleteRoleDialog extends Vue {
  private roleModule = getModule(RoleModule, this.$store)

  @Prop(String)
  private readonly roleId?: string

  private valid: any = false
  private dialog = false
  private loading = false

  private async deleteRole() {
    if (this.roleId) {
      this.loading = true

      await this.roleModule.deleteRole(this.roleId)
    }

    this.dialog = false
  }

}
</script>
