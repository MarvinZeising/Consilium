<template>
  <v-flex
    v-if="canView"
    xs12
  >
    <h2 class="headline mb-3">
      {{ $tc('project.knowledgeBase.articles', 2) }}
    </h2>
    <v-card
      v-if="projectModule.getActiveProject"
      flat
      class="ma-2 mb-5"
      :loading="loading"
    >
      <v-list v-if="knowledgeBaseModule.getActiveTopic">
        <v-list-item v-if="knowledgeBaseModule.getActiveTopic.getArticles.length === 0">
          {{ $tc('project.knowledgeBase.articles', 0) }}
        </v-list-item>
        <v-list-item
          v-for="(article, index) in knowledgeBaseModule.getActiveTopic.getArticles"
          :key="index"
          @click.stop=""
        >
          <v-list-item-content>
            <v-list-item-title v-html="article.title" />
            <v-list-item-subtitle v-html="article.content" />
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import UserModule from '../store/users'
import PersonModule from '../store/persons'
import ProjectModule from '../store/projects'
import KnowledgeBaseModule from '../store/knowledgeBase'
import { Article } from '../models'

@Component
export default class ParticipantsInvitations extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private loading: boolean = true

  private get getArticles() {
    return [{
      id: 'asdf',
      topicId: '1234',
      title: 'My first title',
      content: 'My awesome content'
    }]
  }

  private get canView() {
    return this.personModule.getActiveRole?.knowledgeBaseRead === true
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

    // await this.knowledgeBaseModule.loadArticles();

    this.loading = false
  }

}
</script>
