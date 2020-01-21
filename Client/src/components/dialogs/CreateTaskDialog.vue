<template>
  <v-dialog
    v-model="dialog"
    max-width="1000px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'shift.task.create'"
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
          <v-toolbar-title v-t="'shift.task.create'" />
        </v-toolbar>
        <v-card-text>
          <i
            class="subtitle-1"
            v-t="'shift.task.createDescription'"
          />
        </v-card-text>
        <v-card-text class="pa-2">
          <v-layout wrap>

            <NameControl
              :model="nameModel"
              description="shift.task.nameDescription"
            />

            <TextareaControl
              :model="descriptionModel"
              label="core.description"
              description="shift.task.descriptionDescription"
              :maxLength="1000"
            />

            <TextControl
              :model="helpLinkModel"
              label="shift.task.helpLink"
              description="shift.task.helpLinkDescription"
              :maxLength="1000"
              :customRules="linkRules"
            />

          </v-layout>
        </v-card-text>

        <v-divider />

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
            color="primary"
            v-t="'core.save'"
            :loading="loading"
            :disabled="!valid"
            @click.stop="save"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import TaskModule from '../../store/tasks'
import NameControl from '../controls/NameControl.vue'
import TextControl from '../controls/TextControl.vue'
import TextareaControl from '../controls/TextareaControl.vue'
import { Category, Eligibility, Task } from '../../models'

@Component({
  components: {
    NameControl,
    TextControl,
    TextareaControl,
  },
})
export default class CreateTaskDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private taskModule: TaskModule = getModule(TaskModule, this.$store)

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private task = Task.create({})
  private nameModel = { value: '' }
  private descriptionModel = { value: '' }
  private helpLinkModel = { value: '' }
  private linkRules = [
    (v: string) => !v || /^https:\/\/.+\..{2,}$/.test(v) || i18n.t('shift.task.helpLinkFormat')
  ]

  private async save() {
    if (this.valid) {
      this.loading = true

      this.task.name = this.nameModel.value
      this.task.description = this.descriptionModel.value
      this.task.helpLink = this.helpLinkModel.value

      await this.taskModule.createTask(this.task)

      this.loading = false
      this.dialog = false
    }
  }

}
</script>
