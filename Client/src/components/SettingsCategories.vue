<template>
  <v-flex
    v-if="canView"
    xs12 sm10 md8 lg6 xl4
  >
    <h2 class="headline mb-3">
      {{ $tc('shift.category.categories', 2) }}
    </h2>
    <v-card
      flat
      class="ma-2 mb-5"
      :loading="loading"
    >
      <v-card-text
        class="grey--text"
        v-t="'shift.category.description'"
      />
      <v-list v-if="projectModule.getActiveProject">
        <v-list-item v-if="projectModule.getActiveProject.getCategories.length === 0">
          {{ $tc('shift.category.categories', 0) }}
        </v-list-item>
        <v-list-item
          v-for="(category, index) in projectModule.getActiveProject.getCategories"
          :key="index"
        >
          <v-list-item-content>
            <v-list-item-title v-text="category.name" />
          </v-list-item-content>
          <v-list-item-action>
            <UpdateCategoryDialog :category="category" />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions v-if="!loading">
        <v-spacer />
        <CreateCategoryDialog />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { Role } from '../models'
import CreateCategoryDialog from '../components/dialogs/CreateCategoryDialog.vue'
import UpdateCategoryDialog from '../components/dialogs/UpdateCategoryDialog.vue'
import PersonModule from '../store/persons'
import ProjectModule from '../store/projects'
import CategoryModule from '../store/categories'
import ParticipantModule from '../store/participants'

@Component({
  components: {
    CreateCategoryDialog,
    UpdateCategoryDialog,
  }
})
export default class SettingsCategories extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private categoryModule: CategoryModule = getModule(CategoryModule, this.$store)
  private participantModule: ParticipantModule = getModule(ParticipantModule, this.$store)

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

    await this.categoryModule.loadCategories()

    this.loading = false
  }

}
</script>
