<template>
  <v-container
    fluid
    class="pa-0"
  >
    <v-layout wrap>

      <v-card
        tile
        flat
        width="100%"
        color="accent"
        :loading="loading"
      >
        <v-card-actions>
          <v-btn
            icon
            exact
            :to="getBackLink"
          >
            <v-icon>arrow_back</v-icon>
          </v-btn>
          <v-spacer />
          <UpdateArticleDialog
            v-if="canEdit && knowledgeBaseModule.getActiveArticle"
            :article="knowledgeBaseModule.getActiveArticle"
          />
        </v-card-actions>
        <v-card-title class="mt-5 mb-3 justify-center">
          <h2
            v-if="knowledgeBaseModule.getActiveArticle"
            v-text="knowledgeBaseModule.getActiveArticle.title"
          />
          <v-skeleton-loader
            v-else
            type="heading"
            width="100%"
          />
        </v-card-title>
      </v-card>

      <v-card
        tile
        flat
        class="ma-0"
        width="100%"
      >
        <VueShowdown
          v-if="knowledgeBaseModule.getActiveArticle"
          class="pa-5"
          :markdown="knowledgeBaseModule.getActiveArticle.content"
        />
        <v-skeleton-loader
          v-else
          type="paragraph@10"
          class="ma-5"
        />
      </v-card>

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import axios from 'axios'
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../../store/persons'
import KnowledgeBaseModule from '../../store/knowledgeBase'
import UpdateArticleDialog from '../../components/dialogs/UpdateArticleDialog.vue'

@Component({
  components: {
    UpdateArticleDialog,
  }
})
export default class Article extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private loading: boolean = true

  private get getBackLink() {
    return {
      name: 'topic',
      params: {
        projectId: this.$route.params.projectId,
        topicId: this.$route.params.topicId,
      }
    }
  }

  private get canView() {
    return this.personModule.getActiveRole?.knowledgeBaseRead === true
  }

  private get canEdit() {
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

    await this.knowledgeBaseModule.loadArticle(this.$route.params.articleId);

    this.loading = false
  }

}
</script>
