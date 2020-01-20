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
            v-t="'shift.task.delete'"
          />
        </v-card-title>
        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'shift.task.deleteDescription'"
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
            @click.stop="deleteTask"
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
import TaskModule from '../../store/tasks'
import { Project, Task } from '../../models'

@Component
export default class DeleteTaskDialog extends Vue {
  private taskModule: TaskModule = getModule(TaskModule, this.$store)

  @Prop(Task)
  private readonly task?: Task

  private valid: any = false
  private dialog: boolean = false
  private loading: boolean = false

  private async deleteTask() {
    if (this.task) {
      this.loading = true

      await this.taskModule.deleteTask(this.task.id)

      this.loading = false
    }

    this.dialog = false
  }

}
</script>
