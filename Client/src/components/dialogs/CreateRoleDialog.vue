<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'project.role.create'"
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
            v-t="'project.role.create'"
          />
        </v-card-title>

        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'project.role.createDescription'"
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
            :label="$tc('project.role.roles', 2)"
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

        </v-card-text>
        <v-card-actions>
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
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { VForm } from 'vuetify/lib'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'

@Component
export default class CreateRoleDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private name: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 2 || i18n.t('core.fieldMin', { count: 2 })
  ]
  private settings: any = 'read'
  private roles: any = 'read'
  private participants: any = 'read'
  private knowledgeBase: any = 'read'
  private permissionValues: any[] = [ 'none', 'read', 'write' ].map((value) => {
    return { value }
  })

  private async save() {
    const projectId = this.$route.params.projectId

    if (this.valid) {
      this.loading = true

      await this.projectModule.createRole({
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

}
</script>
