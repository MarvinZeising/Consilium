<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        icon
        class="ma-0"
      >
        <v-icon>edit</v-icon>
      </v-btn>
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

          <p
            class="subtitle-1"
            v-t="'project.role.nameDescription'"
          />
          <v-text-field
            v-model="name"
            :label="$t('core.name')"
            :rules="nameRules"
            filled
            required
          />

          <div v-if="role.editable">
            <p v-t="'project.role.settingsDescription'" />
            <v-select
              v-model="settings"
              :items="permissionValues"
              item-text="name"
              item-value="value"
              :label="$t('project.role.settings')"
              filled
              required
            >
              <template v-slot:selection="{ item }">
                <span>{{ $t('core.' + item.value) }}</span>
              </template>
              <template v-slot:item="{ item }">
                <span>{{ $t('core.' + item.value) }}</span>
              </template>
            </v-select>

            <p v-t="'project.role.rolesDescription'" />
            <v-select
              v-model="roles"
              :items="permissionValues"
              item-text="name"
              item-value="value"
              :label="$t('project.role.roles')"
              filled
              required
            >
              <template v-slot:selection="{ item }">
                <span>{{ $t('core.' + item.value) }}</span>
              </template>
              <template v-slot:item="{ item }">
                <span>{{ $t('core.' + item.value) }}</span>
              </template>
            </v-select>

            <p v-t="'project.role.participantsDescription'" />
            <v-select
              v-model="participants"
              :items="permissionValues"
              item-text="name"
              item-value="value"
              :label="$t('project.role.participants')"
              filled
              required
            >
              <template v-slot:selection="{ item }">
                <span>{{ $t('core.' + item.value) }}</span>
              </template>
              <template v-slot:item="{ item }">
                <span>{{ $t('core.' + item.value) }}</span>
              </template>
            </v-select>

            <p v-t="'project.role.knowledgeBaseDescription'" />
            <v-select
              v-model="knowledgeBase"
              :items="permissionValues"
              item-text="name"
              item-value="value"
              :label="$t('project.role.knowledgeBase')"
              filled
              required
            >
              <template v-slot:selection="{ item }">
                <span>{{ $t('core.' + item.value) }}</span>
              </template>
              <template v-slot:item="{ item }">
                <span>{{ $t('core.' + item.value) }}</span>
              </template>
            </v-select>
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
            @click="dialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="!valid"
            type="submit"
            :loading="loading"
            text
            color="primary"
            @click="save"
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
import { VForm } from 'vuetify/lib'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import PersonModule from '../../store/persons'
import { Role } from '../../models/definitions'
import DeleteRoleDialog from './DeleteRoleDialog.vue'

@Component({
  components: {
    DeleteRoleDialog,
  },
})
export default class UpdateRoleDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  @Prop(Object)
  private readonly role?: Role

  @Prop(Boolean)
  private readonly canBeDeleted?: boolean

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false
  private loadingDelete: boolean = false

  private name: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 2 || i18n.t('core.fieldMin', { count: 2 })
  ]
  private settings: any = 'none'
  private roles: any = 'none'
  private participants: any = 'none'
  private knowledgeBase: any = 'none'
  private permissionValues: any[] = [ 'none', 'read', 'write' ].map((value) => {
    return { value }
  })

  private getPermissionLevel(write: boolean, read: boolean) {
      return write ? 'write' : read ? 'read' : 'none'
  }

  private created() {
    if (this.role) {
      this.name = this.role.name

      if (this.role.editable) {
        this.settings = this.getPermissionLevel(this.role.settingsWrite, this.role.settingsRead)
        this.roles = this.getPermissionLevel(this.role.rolesWrite, this.role.rolesRead)
        this.participants = this.getPermissionLevel(this.role.participantsWrite, this.role.participantsRead)
        this.knowledgeBase = this.getPermissionLevel(this.role.knowledgeBaseWrite, this.role.knowledgeBaseRead)
      }
    }
  }

  private async save() {
    const projectId = this.$route.params.projectId

    if (this.valid && this.role) {
      this.loading = true

      await this.projectModule.updateRole({
        roleId: this.role.id,
        name: this.name,
        settings: this.settings,
        roles: this.roles,
        participants: this.participants,
        knowledgeBase: this.knowledgeBase,
      })

      this.loading = false
      this.dialog = false
    }
  }

  private async remove() {
    const projectId = this.$route.params.projectId

    if (this.valid && this.role) {
      this.loadingDelete = true

      // await this.projectModule.deleteRole(this.role.id)

      this.loadingDelete = false
      this.dialog = false
    }
  }

}
</script>
