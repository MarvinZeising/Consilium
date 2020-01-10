<template>
  <v-dialog
    v-model="dialog"
    max-width="1000px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'project.knowledgeBase.createArticle'"
      />
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'project.knowledgeBase.createArticle'"
          />
        </v-card-title>
        <v-card-text>
            <span
              class="subtitle-1"
              v-t="'project.knowledgeBase.createArticleDescription'"
            />
        </v-card-text>
        <v-card-text>
            <p v-t="'project.knowledgeBase.titleDescription'" />
            <v-text-field
              v-model="title"
              :rules="titleRules"
              :label="$t('project.knowledgeBase.title')"
              counter="100"
              filled
              required
            />
            <p v-t="'project.knowledgeBase.contentDescription'" />
            <v-textarea
              v-model="content"
              :rules="contentRules"
              :label="$t('project.knowledgeBase.content')"
              counter="5000"
              auto-grow
              filled
              required
            />
        </v-card-text>
        <v-card-actions>
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-spacer />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            @click.stop="save"
            :loading="loading"
            :disabled="!valid"
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
import KnowledgeBaseModule from '../../store/knowledgeBase'

@Component
export default class CreateArticleDialog extends Vue {
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private title: string = ''
  private titleRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 100 || i18n.t('core.fieldMax', { count: 100 }),
  ]
  private content: string = ''
  private contentRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 5000 || i18n.t('core.fieldMax', { count: 5000 }),
  ]

  private async save() {
    this.loading = true

    await this.knowledgeBaseModule.createArticle({
      title: this.title,
      content: this.content,
    })

    this.loading = false
    this.dialog = false
  }

}
</script>
