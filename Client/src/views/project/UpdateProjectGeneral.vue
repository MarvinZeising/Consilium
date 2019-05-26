<template>
  <v-container fluid>
    <v-form ref="form">

      <v-flex xs12>
        <h1
          class="headline"
          v-t="'project.updateGeneral'"
        />
      </v-flex>

      <v-flex
        xs12 sm10 md8 lg6
      >
        <p
          class="mt-4 grey--text text--darken-1"
          v-t="'project.nameDescription'"
        />
        <v-text-field
          v-model="name"
          :label="$t('core.name')"
          :rules="nameRules"
          counter="40"
          box
          required
        />

        <p class="mt-4 grey--text text--darken-1">
          {{ $t('project.emailDescription1') }}
          <br>
          {{ $t('project.emailDescription2') }}
        </p>
        <v-text-field
          v-model="email"
          :label="$t('core.email')"
          :rules="emailRules"
          box
          required
        />

        <div class="mt-4">
          <v-btn
            :to="{ name: 'settings' }"
            v-t="'core.cancel'"
          />
          <v-btn
            @click="save"
            type="submit"
            color="primary"
            v-t="'core.save'"
          />
        </div>
      </v-flex>

    </v-form>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import { VForm } from 'vuetify/lib'
import Component from 'vue-class-component'
import ProjectModule from '@/store/modules/projects'
import { getModule } from 'vuex-module-decorators'
import { Project } from '@/models/definitions'
import i18n from '@/i18n'

@Component
export default class UpdateProjectGeneral extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private name: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 3 || i18n.t('core.fieldMin', { count: 3 })
  ]
  private email: string = ''
  private emailRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /.+@.+/.test(v) || i18n.t('core.emailInvalid')
  ]

  private created() {
    const projectId = this.$route.params.projectId
    const project = this.projectModule.myProjects.filter((x: Project) => x.id === projectId)[0]
    this.name = project.name
    this.email = project.email
  }

  private async save() {
    const form: any = this.$refs.form
    const projectId = this.$route.params.projectId

    if (form.validate()) {
      await this.projectModule.updateProjectGeneral({
        id: projectId,
        name: this.name,
        email: this.email
      })
      // TODO: add error handling

      this.$router.back()
    }
  }
}
</script>
