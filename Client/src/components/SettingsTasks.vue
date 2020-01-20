<template>
  <v-flex
    v-if="canView"
    xs12 sm10 md8 lg6 xl4
  >
    <h2 class="headline mb-3">
      {{ $tc('shift.task.tasks', 2) }}
    </h2>
    <v-card
      flat
      class="ma-2 mb-5"
      :loading="loading"
    >
      <v-card-text
        class="grey--text"
        v-t="'shift.task.description'"
      />
      <v-list v-if="projectModule.getActiveProject">
        <!--// TODO: blah -->
        <v-list-item v-if="projectModule.getActiveProject.getTasks.length === 0">
          {{ $tc('shift.task.tasks', 0) }}
        </v-list-item>
        <v-list-item
          v-for="(task, index) in projectModule.getActiveProject.getTasks"
          :key="index"
        >
          <v-list-item-content>
            <v-list-item-title v-text="task.name" />
          </v-list-item-content>
          <v-list-item-action>
            <UpdateTaskDialog :task="task" />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions v-if="!loading">
        <v-spacer />
        <CreateTaskDialog />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { Role } from '../models'
import CreateTaskDialog from '../components/dialogs/CreateTaskDialog.vue'
import UpdateTaskDialog from '../components/dialogs/UpdateTaskDialog.vue'
import PersonModule from '../store/persons'
import ProjectModule from '../store/projects'
import CategoryModule from '../store/categories'
import TaskModule from '../store/tasks'

@Component({
  components: {
    CreateTaskDialog,
    UpdateTaskDialog,
  }
})
export default class SettingsTasks extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private taskModule: TaskModule = getModule(TaskModule, this.$store)

  private loading: boolean = true

  private get canView() {
    return this.personModule.getActiveRole?.calendarWrite === true
  }

  @Watch('personModule.getActivePerson')
  private async onPersonChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  private async created() {
    if (this.canView) {
      await this.init()
    }
  }

  private async init() {
    this.loading = true

    await this.taskModule.loadTasks()

    this.loading = false
  }

}
</script>
