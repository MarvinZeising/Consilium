<template>

  <v-flex
    v-if="canView"
    xs12 sm10 md8 lg6 xl4
  >
    <h2
      class="headline mb-3"
      v-t="'core.general'"
    />
    <v-card
      :loading="loading"
      flat
      class="ma-2 mb-5"
    >

      <!-- //* READ -->
      <v-card-text
        v-if="!editMode"
        class="text--primary"
      >
        <v-layout
          v-if="projectModule.getActiveProject"
          wrap
        >

          <v-flex xs12>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.id'"
            />
            <p class="subtitle-1 grey--text">{{ projectModule.getActiveProject.id }}</p>
          </v-flex>

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

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.createdTime'"
            />
            <p class="subtitle-1 grey--text">
              {{ formatDateTime(projectModule.getActiveProject.createdTime) }}
            </p>
          </v-flex>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.lastUpdatedTime'"
            />
            <p class="subtitle-1 grey--text">
              {{ formatDateTime(projectModule.getActiveProject.lastUpdatedTime) }}
            </p>
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
      <v-card-actions v-if="canEdit">
        <v-spacer />
        <v-btn
          text
          v-if="!editMode"
          @click.stop="toggleEditMode"
          v-t="'core.edit'"
        />
        <v-btn
          text
          v-if="editMode"
          @click.stop="toggleEditMode"
          v-t="'core.cancel'"
        />
        <v-btn
          text
          v-if="editMode"
          color="primary"
          @click.stop="save"
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
import PersonModule from '../store/persons'
import moment from 'moment'

@Component
export default class SettingsGeneral extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private loading: boolean = true
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
    (v: string) => /.+@.+\..+/.test(v) || i18n.t('core.emailInvalid')
  ]

  private get canView() {
    return this.personModule.getActiveRole?.settingsRead === true
  }

  private get canEdit() {
    return this.personModule.getActiveRole?.settingsWrite === true
  }

  private formatDateTime(datetime: string) {
    return moment(datetime).format('ddd, MMM Do YYYY, h:mm a')
    // TODO: store date format string in db
    // TODO: set moment language
  }

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    await this.init()
  }

  private async created() {
    this.init()
  }

  private async init() {
    const projectId = this.$route.params.projectId
    await this.projectModule.loadProject(projectId)

    this.loading = false
  }

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
      // TODO: check that email is correct
      // TODO: maybe by sending an email to verify the new one

      this.toggleEditMode()
    }
  }

}
</script>
