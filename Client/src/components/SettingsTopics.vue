<template>
  <v-flex
    v-if="canView"
    xs12 sm10 md8 lg6 xl4
  >
    <h2 class="headline mb-3">
      {{ $tc('project.knowledgeBase.topics', 2) }}
    </h2>
    <v-card
      flat
      class="ma-2 mb-5"
      :loading="loading"
    >
      <v-card-text
        class="grey--text"
        v-t="'project.knowledgeBase.topicsDescription'"
      />
      <v-list v-if="projectModule.getActiveProject">
        <v-list-item v-if="projectModule.getActiveProject.getTopics.length === 0">
          {{ $tc('project.knowledgeBase.topics', 0) }}
        </v-list-item>
        <v-list-item
          v-for="(topic, index) in projectModule.getActiveProject.getTopics"
          :key="index"
        >
          <v-list-item-content>
            <v-list-item-title v-text="topic.name" />
          </v-list-item-content>

          <v-list-item-action>
            <UpdateTopicDialog :topic="topic" />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-spacer />
        <CreateTopicDialog v-if="!loading" />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../store/persons'
import ProjectModule from '../store/projects'
import KnowledgeBaseModule from '../store/knowledgeBase'
import { Topic } from '../models'
import CreateTopicDialog from './dialogs/CreateTopicDialog.vue'
import UpdateTopicDialog from './dialogs/UpdateTopicDialog.vue'

@Component({
  components: {
    CreateTopicDialog,
    UpdateTopicDialog,
  }
})
export default class SettingsTopics extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private loading: boolean = true

  private get canView() {
    return this.personModule.getActiveRole?.knowledgeBaseWrite === true
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

    await this.knowledgeBaseModule.loadTopics()

    this.loading = false
  }

}
</script>
