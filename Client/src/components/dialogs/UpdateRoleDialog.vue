<template>
  <v-dialog
    v-model="dialog"
    max-width="1000px"
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
        <v-toolbar
          flat
          color="navbar"
        >
          <v-toolbar-title v-t="'project.role.update'" />
        </v-toolbar>
        <v-card-text>
          <i
            class="subtitle-1"
            v-t="'project.role.updateDescription'"
          />
        </v-card-text>
        <v-card-text class="pa-2">

          <NameControl
            :model="nameModel"
            translationPath="project.role.nameDescription"
          />

        </v-card-text>
        <div v-if="role.editable">
          <v-divider />
          <v-card-text class="pa-2">

            <v-layout wrap>
              <v-divider />

              <PermissionControl
                :model="calendarModel"
                translationPath="project.role.calendar"
                @change="(value) => role.setPermissionModel('calendar', value)"
              />

              <PermissionControl
                :model="settingsModel"
                translationPath="project.role.settings"
                @change="(value) => role.setPermissionModel('settings', value)"
              />

              <PermissionControl
                :model="rolesModel"
                translationPath="project.role.roles"
                @change="(value) => role.setPermissionModel('roles', value)"
              />

              <PermissionControl
                :model="participantsModel"
                translationPath="project.role.participants"
                @change="(value) => role.setPermissionModel('participants', value)"
              />

              <PermissionControl
                :model="knowledgeBaseModel"
                translationPath="project.role.knowledgeBase"
                @change="(value) => role.setPermissionModel('knowledgeBase', value)"
              />

            </v-layout>
          </v-card-text>
        </div>

        <v-divider />

        <v-card-text class="pb-0">
          <span class="subtitle-1">
            Trolley
          </span>
        </v-card-text>

        <v-card-text class="pa-2">
          <v-layout wrap>

            <PermissionControl
              :model="knowledgeBaseModel"
              translationPath="project.role.category"
            />

            <v-flex xs12 sm6 md4>
              <v-switch
                v-model="teamCaptainModel"
                :label="$t(`project.role.teamCaptain`)"
                color="primary"
                inset
                hide-details
              />
            </v-flex>

            <v-flex xs12 sm6 md4>
              <v-switch
                v-model="substituteCaptainModel"
                :label="$t(`project.role.substituteCaptain`)"
                color="primary"
                inset
                hide-details
              />
            </v-flex>

          </v-layout>
        </v-card-text>
        <v-divider />
        <v-card-actions>
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <DeleteRoleDialog
            v-if="canBeDeleted && role.editable"
            :roleId="role.id"
          />
          <v-spacer />
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

      await this.roleModule.updateRole(this.role)

      this.loading = false
      this.dialog = false
    }
  }

}
</script>
