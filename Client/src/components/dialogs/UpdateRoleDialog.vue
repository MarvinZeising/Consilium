<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'core.edit'"
      />
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'project.role.update'"
          />
        </v-card-title>
        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'project.role.updateDescription'"
          />

          <NameControl
            :model="nameModel"
            translationPath="project.role.nameDescription"
          />

          <div v-if="role.editable">

            <PermissionControl
              :model="calendarModel"
              translationPath="project.role.calendar"
            />

            <PermissionControl
              :model="settingsModel"
              translationPath="project.role.settings"
            />

            <PermissionControl
              :model="rolesModel"
              translationPath="project.role.roles"
            />

            <PermissionControl
              :model="participantsModel"
              translationPath="project.role.participants"
            />

            <PermissionControl
              :model="knowledgeBaseModel"
              translationPath="project.role.knowledgeBase"
            />

          </div>
        </v-card-text>
        <v-card-actions>
          <DeleteRoleDialog
            v-if="canBeDeleted && role.editable"
            :roleId="role.id"
          />
          <v-spacer />
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="!valid"
            type="submit"
            :loading="loading"
            text
            color="primary"
            @click.stop="save"
            v-t="'core.save'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import PersonModule from '../../store/persons'
import RoleModule from '../../store/roles'
import { Role } from '../../models'
import DeleteRoleDialog from './DeleteRoleDialog.vue'
import NameControl from '../controls/NameControl.vue'
import PermissionControl from '../controls/PermissionControl.vue'

@Component({
  components: {
    DeleteRoleDialog,
    NameControl,
    PermissionControl,
  },
})
export default class UpdateRoleDialog extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private roleModule: RoleModule = getModule(RoleModule, this.$store)

  @Prop(Object)
  private readonly role?: Role

  @Prop(Boolean)
  private readonly canBeDeleted?: boolean

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private nameModel: { value: string } = { value: '' }
  private calendarModel: { value: string } = { value: 'none' }
  private settingsModel: { value: string } = { value: 'none' }
  private rolesModel: { value: string } = { value: 'none' }
  private participantsModel: { value: string } = { value: 'none' }
  private knowledgeBaseModel: { value: string } = { value: 'none' }

  private getPermissionLevel(write: boolean, read: boolean) {
      return write ? 'write' : read ? 'read' : 'none'
  }

  private created() {
    if (this.role) {
      this.nameModel = { value: this.role.name }

      if (this.role.editable) {
        this.calendarModel = { value: this.getPermissionLevel(this.role.calendarWrite, this.role.calendarRead) }
        this.settingsModel = { value: this.getPermissionLevel(this.role.settingsWrite, this.role.settingsRead) }
        this.rolesModel = { value: this.getPermissionLevel(this.role.rolesWrite, this.role.rolesRead) }
        this.participantsModel = { value: this.getPermissionLevel(this.role.participantsWrite, this.role.participantsRead) }
        this.knowledgeBaseModel = { value: this.getPermissionLevel(this.role.knowledgeBaseWrite, this.role.knowledgeBaseRead) }
      }
    }
  }

  private async save() {
    if (this.valid && this.role) {
      this.loading = true

      await this.roleModule.updateRole({
        roleId: this.role.id,
        name: this.nameModel.value,
        calendar: this.calendarModel.value,
        settings: this.settingsModel.value,
        roles: this.rolesModel.value,
        participants: this.participantsModel.value,
        knowledgeBase: this.knowledgeBaseModel.value,
      })

      this.loading = false
      this.dialog = false
    }
  }

}
</script>
