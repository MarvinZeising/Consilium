<template>

  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'core.general'"
    />
    <v-card
      v-if="projectModule.getActiveProject"
      flat
      class="ma-2 mb-5"
    >

      <!-- //* READ -->
      <v-card-text
        v-if="!editMode"
        class="text--primary"
      >
        <v-layout wrap>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.name'"
            />
            <p class="subtitle-1">{{ projectModule.getActiveProject.name }}</p>
          </v-flex>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.email'"
            />
            <p class="subtitle-1">{{ projectModule.getActiveProject.email }}</p>
          </v-flex>

        </v-layout>
      </v-card-text>

      <!-- //* UPDATE -->
      <v-card-text v-else>
        <v-form ref="form">
          <p v-t="'project.nameDescription'" />
          <v-text-field
            v-model="name"
            :label="$t('core.name')"
            :rules="nameRules"
            counter="40"
            filled
            required
          />
          <p v-t="'project.emailDescription1'" />
          <p v-t="'project.emailDescription2'" />
          <v-text-field
            v-model="email"
            :label="$t('core.email')"
            :rules="emailRules"
            filled
            required
          />
        </v-form>
      </v-card-text>

      <!-- //* ACTIONS -->
      <v-card-actions>
        <v-spacer />
        <v-btn
          text
          v-if="!editMode"
          @click="toggleEditMode"
          v-t="'core.edit'"
        />
        <v-btn
          text
          v-if="editMode"
          @click="toggleEditMode"
          v-t="'core.cancel'"
        />
        <v-btn
          text
          v-if="editMode"
          color="primary"
          @click="save"
          v-t="'core.save'"
        />
      </v-card-actions>
    </v-card>
  </v-flex>

</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { VForm } from 'vuetify/lib'
import i18n from '../i18n'
import ProjectModule from '../store/projects'

@Component
export default class SettingsGeneral extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private editMode: boolean = false

  private name: string = this.projectModule.getActiveProject?.name || ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 3 || i18n.t('core.fieldMin', { count: 3 })
  ]
  private email: string = this.projectModule.getActiveProject?.email || ''
  private emailRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /.+@.+/.test(v) || i18n.t('core.emailInvalid')
  ]

  private toggleEditMode() {
    this.editMode = !this.editMode

    if (this.editMode) {
      this.name = this.projectModule.getActiveProject?.name || ''
      this.email = this.projectModule.getActiveProject?.email || ''
    }
  }

  private async save() {
    const form: any = this.$refs.form
    const userName = this.projectModule.getActiveProject?.name || ''

    if (form.validate()) {
      const projectId = this.$route.params.projectId

      await this.projectModule.updateProjectGeneral({
        id: projectId,
        name: this.name,
        email: this.email
      })
      // TODO: add error handling
      // TODO: check that email is correct
      // TODO: maybe by sending an email to verify the new one

      this.toggleEditMode()
    }
  }

}
</script>
